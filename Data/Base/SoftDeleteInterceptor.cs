using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data.Base
{

    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges( DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;

                entry.State = EntityState.Modified;
                delete.Deleted = true;
                delete.DeletedAt = DateTimeOffset.UtcNow;
            }

            return result;
        }
        //public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        //    DbContextEventData eventData,
        //    InterceptionResult<int> result,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);
        //    return new ValueTask<InterceptionResult<int>>(result);
        //}

        public override   ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) new ValueTask<InterceptionResult<int>>(result);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;

                entry.State = EntityState.Modified;
                delete.Deleted = true;
                delete.DeletedAt = DateTimeOffset.UtcNow;
            }

            return new ValueTask<InterceptionResult<int>>(result);
        }
        
    }
}
