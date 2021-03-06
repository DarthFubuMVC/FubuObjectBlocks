﻿using FubuTestingSupport;
using NUnit.Framework;

namespace FubuObjectBlocks.Tests
{
    [TestFixture]
    public class block_for_object_with_nested_types
    {
        [Test]
        public void sets_the_properties()
        {
            var target = new ComplexTarget
            {
                Name = "test",
                Nested = new NestedTarget {Email = "test@test.com"}
            };

            var writer = ObjectBlockWriter.Basic();

            var block = writer.BlockFor(target);

            block.FindProperty("name").Value.ShouldEqual("test");

            var nested = block.FindNested("nested");
            nested.FindProperty("email").Value.ShouldEqual("test@test.com");
        }

        public class ComplexTarget
        {
            public string Name { get; set; }
            public NestedTarget Nested { get; set; }
        }
        
        public class NestedTarget
        {
            public string Email { get; set; }
        }
    }

    [TestFixture]
    public class block_for_object_with_ignored_properties
    {
        [Test]
        public void excludes_the_ignored_properties()
        {
            var target = new ComplexTarget
            {
                Name = "test",
                Nested = new NestedTarget { Email = "test@test.com" }
            };

            var writer = ObjectBlockWriter.Basic(x => x.RegisterSettings<ComplexTargetSettings>());

            var block = writer.BlockFor(target);

            block.FindProperty("name").Value.ShouldEqual("test");

            block.FindNested("nested").ShouldBeNull();
        }

        public class ComplexTargetSettings : ObjectBlockSettings<ComplexTarget>
        {
            public ComplexTargetSettings()
            {
                Ignore(x => x.Nested);
            }
        }

        public class ComplexTarget
        {
            public string Name { get; set; }
            public NestedTarget Nested { get; set; }
        }

        public class NestedTarget
        {
            public string Email { get; set; }
        }
    }
}