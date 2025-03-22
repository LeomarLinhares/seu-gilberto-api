using System.Collections.Generic;
using PainelGilberto.DTOs;
using PainelGilberto.Models;

namespace PainelGilberto.Services
{
    public interface IGeneralInfoService
    {
        IEnumerable<Round> GetRoundsBySeason();
        DashboardDTO GetAllDashboardData(int seasonId);

    }
}
