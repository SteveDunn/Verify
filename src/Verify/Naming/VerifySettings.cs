﻿using System.Runtime.CompilerServices;

namespace VerifyTests
{
    public partial class VerifySettings
    {
        internal Namer Namer = new();

        public void UniqueForAssemblyConfiguration()
        {
            Namer.UniqueForAssemblyConfiguration = true;
        }

        public void UniqueForRuntime()
        {
            Namer.UniqueForRuntime = true;
        }

        internal string? directory;

        public void UseDirectory(string directory)
        {
            Guard.AgainstNullOrEmpty(directory, nameof(directory));
            this.directory = directory;
        }

        internal string? typeName;

        public void UseTypeName(string name)
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));
            ThrowIfFileNameDefined();

            typeName = name;
        }

        internal string? methodName;

        public void UseMethodName(string name)
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));
            ThrowIfFileNameDefined();

            methodName = name;
        }

        internal string? fileName;

        public void UseFileName(string fileName)
        {
            Guard.AgainstNullOrEmpty(fileName, nameof(fileName));
            ThrowIfMethodOrTypeNameDefined();

            this.fileName = fileName;
        }

        void ThrowIfMethodOrTypeNameDefined()
        {
            if (methodName != null ||
                typeName != null)
            {
                throw new($"{nameof(UseFileName)} is not compatible with {nameof(UseMethodName)} or {nameof(UseTypeName)}.");
            }
        }


        void ThrowIfFileNameDefined([CallerMemberName] string caller = "")
        {
            if (fileName != null)
            {
                throw new($"{caller} is not compatible with {nameof(UseFileName)}.");
            }
        }

        public void UniqueForRuntimeAndVersion()
        {
            Namer.UniqueForRuntimeAndVersion = true;
        }
    }
}