namespace Inventario.Api.Services;
using Inventario.Api.Models;
 
public  interface ICategoriaService
{
    IEnumerable<Categoria> GetAll();
    Categoria? getById(int  id);
   
    Categoria CreateCategory(string nombre);
    bool UpdateCat(int id, string name);
    bool DeleteCategoria(int id);
    bool ExistName(string name);
}