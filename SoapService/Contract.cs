using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace SoapService;

[Serializable]
public class ContractModel
{
    [XmlElement(ElementName = "Message", Namespace = "http://schemas.datacontract.org/2004/07/")]
    public string? Message { get; set; }
}

[ServiceContract]
public interface IContractService
{
    [OperationContract(Action = "http://mysoapaction")]
    ContractModel Endpoint(ContractModel contractModel);
}

public class Contract : IContractService
{
    public ContractModel Endpoint(ContractModel contractModel)
    {
        return contractModel;
    }
}
