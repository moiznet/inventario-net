namespace Inventario.Api.Services;
using Inventario.Api.Models;
 
public  interface ICategoriaService
{
    IEnumerable<Categoria> GetAll();
    Categoria? ObtenerPorId(int  id);
   
    Categoria CreateCategory(string nombre);
    bool UpdateCat(int id, string name);
    bool DeleteCategoria(int id);
    bool ExistName(string name);
}