﻿using System;
using FubuCore;
using FubuCore.Binding;
using FubuCore.Binding.InMemory;

namespace FubuObjectBlocks
{
    public class ObjectBlockReader : IObjectBlockReader
    {
        private readonly IObjectBlockParser _parser;
        private readonly IObjectResolver _resolver;
        private readonly IServiceLocator _services;
        private readonly BlockRegistry _blocks;

        public ObjectBlockReader(IObjectBlockParser parser, IObjectResolver resolver, IServiceLocator services, BlockRegistry blocks)
        {
            _parser = parser;
            _resolver = resolver;
            _services = services;
            _blocks = blocks;
        }

        public T Read<T>(string input)
        {
            return Read(typeof(T), input).As<T>();
        }

        public void Read<T>(T target, string input)
        {
            var settings = _blocks.SettingsFor(typeof(T));
            var block = _parser.Parse(input, settings);
            var data = new RequestData(new ObjectBlockValues(block, settings, typeof (T)));
            var context = new BindingContext(data, _services, new NulloBindingLogger());

            _resolver.BindProperties(target, context);
        }

        public object Read(Type type, string input)
        {
            var settings = _blocks.SettingsFor(type);
            var block = _parser.Parse(input, settings);

            return Read(type, block);
        }

        public object Read(Type type, ObjectBlock block)
        {
            var settings = _blocks.SettingsFor(type);
            block.MakeCollections(settings);
            var result = _resolver.BindModel(type, new ObjectBlockValues(block, settings, type));

            return result.Value;
        }

        public ObjectBlock Read(string input)
        {
            return _parser.Parse(input, new ObjectBlockSettings());
        }

        public static ObjectBlockReader Basic()
        {
            return Basic(BlockRegistry.Basic());
        }

        public static ObjectBlockReader Basic(Action<BlockRegistry> configure)
        {
            var registry = BlockRegistry.Basic();
            configure(registry);

            return Basic(registry);
        }

        public static ObjectBlockReader Basic(BlockRegistry registry)
        {
            return new ObjectBlockReader(new ObjectBlockParser(), ObjectResolver.Basic(), new InMemoryServiceLocator(), registry);
        }
    }
}