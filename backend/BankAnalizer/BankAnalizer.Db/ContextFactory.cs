namespace BankAnalizer.Db
{
    public interface IContextFactory
    {
        IContext GetContext();
    }

    public class ContextFactory : IContextFactory
    {
        private readonly IConnectionFactory connectionFactory;

        public ContextFactory(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IContext GetContext()
        {
            return new PkoContext(connectionFactory);
        }
    }
}
