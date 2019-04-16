using System;
using Fusillade;

namespace TemplatePrism.Services.Api
{
    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
