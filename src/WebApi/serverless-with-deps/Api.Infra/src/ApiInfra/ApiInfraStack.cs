using Amazon.CDK;
using Constructs;

namespace ApiInfra
{
    public class ApiInfraStack : Stack
    {
        internal ApiInfraStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var networkStack = new NetworkingStack(this, "NetworkingStack");

            var persistenceStack = new PersistenceStack(this, "PersistenceStack",
                new PersistenceStackProps(networkStack.Vpc, networkStack.ApplicationSecurityGroup));
        }
    }
}
