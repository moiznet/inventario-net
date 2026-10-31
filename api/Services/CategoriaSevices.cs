namespace Inventario.Api.Services;
using Inventario.Api.Services;
using Inventario.Api.Models;


public class CategoriaServices(ILogger<ProductoServices> logger): ICategoriaService 
{   

    private readonly List<Categoria> _Categorias = [
        new Categoria(1,"Periféricos"),
        new Categoria(2,"Pantallas")
    ];
    public IEnumerable<Categoria> GetAll()
    {
        return _Categorias;

    }
    public Categoria? getById(int id)
    {
        return _Categorias.FirstOrDefault(p => p.Id == id);
    }

    

    public Categoria CreateCategory(string nombre)
    {
        var NuevoId = _Categorias.Count == 0 ? 1 : _Categorias.Max(p => p.Id) + 1;
        var NuevaCategoria = new Categoria(NuevoId,nombre);
        _Categorias.Add(NuevaCategoria);
        logger.LogInformation("Categoria creada: {NuevoId} {nombre}", NuevaCategoria.Id, NuevaCategoria.Nombre);
        return NuevaCategoria;

    }

    public bool UpdateCat(int id, string name)
    {
        var categoria = getById(id);
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
        var ExistName = _Categorias.Where(p=> p.Nombre == nombre).ToList();

        if (ExistName.Count <= 0 )
        {
            return true;
        }
        else
        {
            return false;
        }
    }




}