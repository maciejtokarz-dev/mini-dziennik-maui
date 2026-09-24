using SQLite;
using MiniDziennik.Models;


namespace MiniDziennik.Services;

public class WpisService
{
    private SQLiteAsyncConnection _db;

    private async Task InitAsync()
    {
        if (_db != null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Dziennik.db3");
        _db = new SQLiteAsyncConnection(dbPath);

        await _db.CreateTableAsync<Wpis>();
    }

    public async Task<List<Wpis>> PobierzWszystkieAsync()
    {
        await InitAsync();
        return await _db.Table<Wpis>()
                        .OrderByDescending(x => x.DataUtworzenia)
                        .ToListAsync();
    }

    public async Task<Wpis?> PobierzPoIdAsync(int id)
    {
        await InitAsync();
        return await _db.Table<Wpis>()
                        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DodajAsync(Wpis wpis)
    {
        await InitAsync();
        await _db.InsertAsync(wpis);
    }

    public async Task AktualizujAsync(Wpis wpis)
    {
        await InitAsync();
        await _db.UpdateAsync(wpis);
    }

    public async Task UsunAsync(int id)
    {
        await InitAsync();
        var wpis = await PobierzPoIdAsync(id);
        if (wpis != null)
        {
            await _db.DeleteAsync(wpis);
        }
    }
}
