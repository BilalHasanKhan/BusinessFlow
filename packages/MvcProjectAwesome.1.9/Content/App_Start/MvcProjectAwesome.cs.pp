using System.Web.Mvc;
using Omu.Awesome.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.MvcProjectAwesome), "Start")]
namespace $rootnamespace$.App_Start
{    
    public static class MvcProjectAwesome
    {
        public static void Start()
        {
            ModelMetadataProviders.Current = new AwesomeModelMetadataProvider();
        }
    }
}