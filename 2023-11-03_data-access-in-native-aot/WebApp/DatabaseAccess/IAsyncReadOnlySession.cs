using System;

namespace WebApp.DatabaseAccess;

public interface IAsyncReadOnlySession : IAsyncDisposable, IDisposable { }