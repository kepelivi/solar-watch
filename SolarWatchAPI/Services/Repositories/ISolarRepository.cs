using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.Repositories;

public interface ISolarRepository
{
    public Solar GetSolar(int cityId);

    public void AddSolar(Solar solar);
    public void DeleteSolar(int id);
    public void UpdateSolar(Solar solar);
}