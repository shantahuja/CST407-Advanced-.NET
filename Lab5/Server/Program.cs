using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    //static void FromCodeInsteadOfConfigFile()
    //{
    //    ServiceHost host = new ServiceHost(typeof(ExampleService),
    //        new Uri("net.tcp://localhost:8731/"),
    //        new Uri("http://localhost:8732/"));

    //    host.AddServiceEndpoint(typeof(IService),
    //                            new NetTcpBinding(),
    //                            "Example");

    //    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
    //    behavior.HttpGetEnabled = true;
    //    host.Description.Behaviors.Add(behavior);

    //    host.AddServiceEndpoint(
    //        ServiceMetadataBehavior.MexContractName,
    //        MetadataExchangeBindings.CreateMexHttpBinding(),
    //        "mex");
    //}
}

