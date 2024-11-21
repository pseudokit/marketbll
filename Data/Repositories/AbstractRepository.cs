using Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public abstract class AbstractRepository
    {
        private protected readonly TradeMarketDbContext context;

        protected AbstractRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
    }
}
