using BambooCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataTransferHelper.Transfer
{
    public class SolutionTransfer
    {
        public static async Task Transfer(string sourceOrganId, string targetUserId)
        {
            var context = new ApiDbContext();
            var targetUser = await context.Accounts.FindAsync(targetUserId);
            var solutions = context.Solutions.Where(x => x.OrganizationId == sourceOrganId).ToList();
            for (int idx = solutions.Count - 1; idx >= 0; idx--)
            {
                var curSolution = solutions[idx];
                curSolution.Id = GuidGen.NewGUID();
                curSolution.Creator = targetUserId;
                curSolution.Modifier = targetUserId;
                curSolution.OrganizationId = targetUser.OrganizationId;
                context.Solutions.Add(curSolution);
            }
            await context.SaveChangesAsync();
        }
    }
}
