namespace Hydra.Cooling.Alerts
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof(SmsGatewayContract))]
    public interface ISmsGateway
    {
        Task<SmsTransmissionProtocol> SendAsync(SmsMessage message);
    }

    [ContractClassFor(typeof(ISmsGateway))]
    internal abstract class SmsGatewayContract : ISmsGateway
    {
        public Task<SmsTransmissionProtocol> SendAsync(SmsMessage message)
        {
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<SmsTransmissionProtocol>() != null);

            return Task<SmsTransmissionProtocol>.Factory.StartNew(() => SmsTransmissionProtocol.Empty);
        }
    }
}