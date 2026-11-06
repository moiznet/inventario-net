namespace Inventario.Api.Services;
using Inventario.Api.Services;
using Inventario.Api.Models;


public class CategoriaServices(ILogger<CategoriaServices> logger): ICategoriaService 
{   

    private readonly List<Categoria> _Categorias = [
        // new Categoria(1,"Periféricos"),
        // new Categoria(2,"Pantallas")
    ];
    public IEnumerable<Categoria> GetAll()
    {
        return _Categorias;

    }
    public Categoria? ObtenerPorId(int id)
    {
        return _Categorias.FirstOrDefault(p => p.Id == id);
    }

    

    public Categoria CreateCategory(string nombre)
    {
        throw new NotImplementedException("Se reescribe con EF Core en el tema 5");

    }

    public bool UpdateCat(int id, string name)
    {
        var categoria = ObtenerPorId(id);
        if(categoria == null)
        {
            return false;
        }
        
            categoria.RenombrarCategoria(name);
            return true;
        
        

    }

    public bool  DeleteCategoria(int id)
    {
        var categoria = _Categorias.FirstOrDefault(p => p.Id == id);
        if (categoria is null)
        return false;
         
        return _Categorias.Remove(categoria);
       
        

    }

    public bool ExistName(string nombre)
    {
        return _Categorias.Any(c => string.Equals(c.Nombre, nombre, StringComparison.OrdinalIgnoreCase));

    }




}