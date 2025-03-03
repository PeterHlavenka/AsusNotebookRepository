using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;

namespace DotvvmProject.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {

		public string Title { get; set;}

		public DefaultViewModel()
		{
			Title = "Hello from DotVVM!";
			// zde je k dispozici trida Context, diky ktere muzeme provadet treba redirect na jinou stranku
		}
		
		// zde jsou napriklad commandy
		public void SayHello()
		{
			Title = "Hello world";
		}

    }
}
