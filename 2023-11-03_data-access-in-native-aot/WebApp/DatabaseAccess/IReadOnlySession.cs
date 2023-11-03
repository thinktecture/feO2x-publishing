using System;

namespace WebApp.DatabaseAccess;

public interface IReadOnlySession : IAsyncDisposable, IDisposable { }