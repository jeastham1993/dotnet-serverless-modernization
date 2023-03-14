using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Constructs;

namespace ApiInfra;

public record PersistenceStackProps(IVpc Vpc, ISecurityGroup AppSecurityGroup);

public class PersistenceStack : Construct
{
    public PersistenceStack(Construct scope, string id, PersistenceStackProps props) : base(scope, id)
    {
        var secret = CreateDbSecret();
        var dbSg = CreateDatabaseSecurityGroup(props.Vpc);

        var dbInstance = new DatabaseInstance(this, "MsSQLDatabaseInstance", new DatabaseInstanceProps()
        {
            Vpc = props.Vpc,
            AllowMajorVersionUpgrade = false,
            BackupRetention = Duration.Days(0),
            InstanceIdentifier = "modernisedapidb",
            VpcSubnets = new SubnetSelection
            {
                SubnetType = SubnetType.PRIVATE_WITH_EGRESS
            },
            SecurityGroups = new []{dbSg},
            Engine = DatabaseInstanceEngine.SQL_SERVER_EX,
            InstanceType = InstanceType.Of(InstanceClass.T3, InstanceSize.SMALL),
            Credentials = Credentials.FromSecret(secret)
        });
        
        dbInstance.Connections.AllowFromAnyIpv4(Port.Tcp(1433));
        dbInstance.Connections.AllowFrom(dbInstance.Connections.SecurityGroups[0], Port.Tcp(1433));
        dbInstance.Connections.AllowFrom(props.AppSecurityGroup, Port.Tcp(1433));

        var output = new CfnOutput(this, "SecretARN", new CfnOutputProps()
        {
            ExportName = "SecretFullARN",
            Value = secret.SecretFullArn
        });
    }

    private DatabaseSecret CreateDbSecret()
    {
        return new DatabaseSecret(this, "RdsDbSecret", new DatabaseSecretProps()
        {
            SecretName = "rdsdbsecret",
            Username = "mssqluser"
        });
    }

    private ISecurityGroup CreateDatabaseSecurityGroup(IVpc vpc)
    {
        var dbSg = new SecurityGroup(this, "DatabaseSecurityGroup", new SecurityGroupProps()
        {
            SecurityGroupName = "DatabaseSG",
            AllowAllOutbound = false,
            Vpc = vpc
        });
        
        dbSg.AddIngressRule(Peer.Ipv4(vpc.VpcCidrBlock), Port.Tcp(1433), "Allow MSSQL Database Traffic from local network");

        return dbSg;
    }
}