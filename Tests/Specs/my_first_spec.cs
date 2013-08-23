using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

namespace Tests.Specs
{
    public class my_first_spec : nspec
    {
        void given_the_world_has_not_come_to_an_end()
        {
            // NSpecRunner.exe Tests\bin\Debug\Tests.dll
            it["Hello World should be Hello World"] = () => "Hello World".should_be("Hello World");
        }
    }
}
