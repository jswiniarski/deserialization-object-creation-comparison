using System;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using AutoFixture;

namespace Deserialization
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckNewtonsoftJson();
            CheckMongoDbBson();
            CheckSpecFlow();
            CheckAutofixture();    
        }

        static void CheckNewtonsoftJson()
        {
            var json = @"{
                'GetOnlyProp': 'asd',
                'PrivateSetProp': 'qwe',
                'InitProp': 'zxc',
                'FullProp': 'vbn'
            }";
            
            var obj1 = JsonConvert.DeserializeObject<DefaultOnlyConstructorClass>(json);
            Console.WriteLine("+++++ Newtonsoft.Json +++++");
            Console.WriteLine("===== DefaultOnlyConstructorClass =====");
            obj1.Dump();
            
            var obj2 = JsonConvert.DeserializeObject<ParamOnlyConstructorClass>(json);
            Console.WriteLine("===== ParamOnlyConstructorClass =====");
            obj2.Dump();
            
            var obj3 = JsonConvert.DeserializeObject<DefaultAndParamConstructorClass>(json);
            Console.WriteLine("===== DefaultAndParamConstructorClass =====");
            obj3.Dump();
        }

        static void CheckMongoDbBson()
        {
            var doc = BsonDocument.Parse(@"{
                GetOnlyProp: 'asd',
                PrivateSetProp: 'qwe',
                InitProp: 'zxc',
                FullProp: 'vbn'
            }");

            BsonClassMap.RegisterClassMap<DefaultOnlyConstructorClass>(cm => 
            {
                cm.AutoMap();
                //cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<ParamOnlyConstructorClass>(cm => 
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<DefaultAndParamConstructorClass>(cm => 
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            Console.WriteLine("+++++ MongoDB.Bson +++++");

            var obj1 = BsonSerializer.Deserialize<DefaultOnlyConstructorClass>(doc);
            Console.WriteLine("===== DefaultOnlyConstructorClass =====");
            obj1.Dump();

            var obj2 = BsonSerializer.Deserialize<ParamOnlyConstructorClass>(doc);
            Console.WriteLine("===== ParamOnlyConstructorClass =====");
            obj2.Dump();

            var obj3 = BsonSerializer.Deserialize<DefaultAndParamConstructorClass>(doc);
            Console.WriteLine("===== DefaultAndParamConstructorClass =====");
            obj3.Dump();
        }

        static void CheckSpecFlow()
        {
            var tab = new Table(
                //nameof(Base.GetOnlyProp), 
                nameof(Base.PrivateSetProp),
                nameof(Base.InitProp),
                nameof(Base.FullProp));

            tab.AddRow(
                //"qwe", 
                "asd", 
                "zxc", 
                "vbn");

            var obj1 = tab.CreateInstance<DefaultOnlyConstructorClass>();
            Console.WriteLine("+++++ SpecFlow CreateInstance +++++");
            Console.WriteLine("===== DefaultOnlyConstructorClass =====");
            obj1.Dump();
            
            var obj2 = tab.CreateInstance<ParamOnlyConstructorClass>();
            Console.WriteLine("===== ParamOnlyConstructorClass =====");
            obj2.Dump();
            
            var obj3 = tab.CreateInstance<DefaultAndParamConstructorClass>();
            Console.WriteLine("===== DefaultAndParamConstructorClass =====");
            obj3.Dump();
        }

        static void CheckAutofixture()
        {
            var fixture = new Fixture();

            var obj1 = fixture.Create<DefaultOnlyConstructorClass>();
            Console.WriteLine("+++++ Autofixture Create +++++");
            Console.WriteLine("===== DefaultOnlyConstructorClass =====");
            obj1.Dump();
            
            var obj2 = fixture.Create<ParamOnlyConstructorClass>();
            Console.WriteLine("===== ParamOnlyConstructorClass =====");
            obj2.Dump();
            
            var obj3 = fixture.Create<DefaultAndParamConstructorClass>();
            Console.WriteLine("===== DefaultAndParamConstructorClass =====");
            obj3.Dump();
        }
    }

    public class Base
    {
        private int privateSetterCalled = 0;
        private int initSetterCalled = 0;
        private int publicSetterCalled = 0;

        protected string getOnlyProp;
        private string privateSetProp;
        private string initProp;
        private string fullProp;

        public string GetOnlyProp => getOnlyProp; 
        public string PrivateSetProp
        {
            get => privateSetProp;
            protected set
            {
                privateSetProp = value;
                privateSetterCalled++;
            }
        }
        public string InitProp
        {
            get => initProp;
            init
            {
                initProp = value;
                initSetterCalled++;
            }
        }
        public string FullProp
        {
            get => fullProp;
            set
            {
                fullProp = value;
                publicSetterCalled++;
            }
        }

        public virtual void Dump()
        {
            Console.WriteLine(new string('-', 50));

            Console.WriteLine($"{nameof(GetOnlyProp)}: {GetOnlyProp}");
            Console.WriteLine($"{nameof(PrivateSetProp)}: {PrivateSetProp}");
            Console.WriteLine($"{nameof(InitProp)}: {InitProp}");
            Console.WriteLine($"{nameof(FullProp)}: {FullProp}");

            Console.WriteLine(new string('-', 50));

            Console.WriteLine($"{nameof(privateSetterCalled)}: {privateSetterCalled}");
            Console.WriteLine($"{nameof(initSetterCalled)}: {initSetterCalled}");
            Console.WriteLine($"{nameof(publicSetterCalled)}: {publicSetterCalled}");
        }
    }

    public class DefaultOnlyConstructorClass : Base
    {
        private int defaultConsCalled = 0;

        public DefaultOnlyConstructorClass()
        {
            defaultConsCalled++;
        }

        public override void Dump()
        {
            base.Dump();
            Console.WriteLine(new string('-', 50));

            Console.WriteLine($"{nameof(defaultConsCalled)}: {defaultConsCalled}");
        }
    }

    public class ParamOnlyConstructorClass : Base
    {
        private int paramConsCalled = 0;

        public ParamOnlyConstructorClass(
            string getOnlyProp, 
            string privateSetProp,
            string initProp,
            string fullProp)
        {
            this.getOnlyProp = getOnlyProp;
            PrivateSetProp = privateSetProp; 
            InitProp = initProp;
            FullProp = fullProp;

            paramConsCalled++;
        }

        public override void Dump()
        {
            base.Dump();
            
            Console.WriteLine($"{nameof(paramConsCalled)}: {paramConsCalled}");
        }
    }

    public class DefaultAndParamConstructorClass : Base
    {
        private int defaultConsCalled = 0;
        private int paramConsCalled = 0;

        public DefaultAndParamConstructorClass()
        {
            defaultConsCalled++;
        }

        public DefaultAndParamConstructorClass(
            string getOnlyProp, 
            string privateSetProp,
            string initProp,
            string fullProp)
        {
            this.getOnlyProp = getOnlyProp;
            PrivateSetProp = privateSetProp; 
            InitProp = initProp;
            FullProp = fullProp;

            paramConsCalled++;
        }

        public override void Dump()
        {
            base.Dump();
            Console.WriteLine(new string('-', 50));

            Console.WriteLine($"{nameof(defaultConsCalled)}: {defaultConsCalled}");
            Console.WriteLine($"{nameof(paramConsCalled)}: {paramConsCalled}");
        }
    }
}
