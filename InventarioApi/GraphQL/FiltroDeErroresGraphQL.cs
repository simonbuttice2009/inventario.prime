using HotChocolate;
using HotChocolate.Execution; // Asegúrate de incluir este
using InventarioApi.Exceptions;

namespace InventarioApi.GraphQL;

public class FiltroDeErroresGraphQL : IErrorFilter
{
    public IError OnError(IError error) => error.Exception switch
    {
        NoEncontradoException ex => ErrorBuilder.FromError(error)
            .SetMessage(ex.Message).SetCode("NO_ENCONTRADO").Build(),

        ReglaDeNegocioException ex => ErrorBuilder.FromError(error)
            .SetMessage(ex.Message).SetCode("REGLA_DE_NEGOCIO").Build(),

        _ => error
    };
}