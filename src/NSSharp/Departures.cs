using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSSharp
{
    public interface Departures
    {
        Task<IReadOnlyList<Departure>> All(DepartureRequest request);
    }
}