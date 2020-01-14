namespace PkoAnalizer.Db
{
    public interface IContextFactory
    {
        IContext GetContext();
    }

    public class ContextFactory : IContextFactory
    {
        private readonly ConnectionFactory connectionFactory;

        public ContextFactory(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IContext GetContext()
        {
            return new PkoContext(connectionFactory);
        }
    }
}
