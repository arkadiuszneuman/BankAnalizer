using Microsoft.EntityFrameworkCore;

namespace BankAnalizer.Db
{
    public interface IContextFactory
    {
        IContext GetContext();
    }

    public class ContextFactory : IContextFactory
    {
        private readonly DbContextOptions<BankAnalizerContext> options;

        public ContextFactory(DbContextOptions<BankAnalizerContext> options)
        {
            this.options = options;
        }

        public IContext GetContext()
        {
            return new BankAnalizerContext(options);
        }
    }
}
