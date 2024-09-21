using System;
using cshttp.constants;

namespace cshttp.Tests;

public class RouterTest
{
    class RetrievedException : Exception { }
    class CreatedException : Exception { }

    [Fact]
    public void shouldRouteAndFindRoute()
    {
        Router router = new Router();

        String route1 = "/path/to/resource";
        String route2 = "/path/to/another/resource";
        String route3 = "/";

        router.route(HttpMethod.GET, route1, (_, _) =>
        {
            throw new RetrievedException();
        });
        router.route(HttpMethod.POST, route1, (_, _) =>
        {
            throw new CreatedException();
        });

        router.route(HttpMethod.GET, route2, (_, _) =>
        {
            throw new RetrievedException();
        });
        router.route(HttpMethod.POST, route2, (_, _) =>
        {
            throw new CreatedException();
        });

        router.route(HttpMethod.POST, route3, (_, _) =>
        {
            throw new CreatedException();
        });
        router.route(HttpMethod.GET, route3, (_, _) =>
        {
            throw new RetrievedException();
        });


        Assert.Throws<RetrievedException>(() => router.lookUp(route1)!.handlers![HttpMethod.GET](null!, null!));
        Assert.Throws<CreatedException>(() => router.lookUp(route1)!.handlers![HttpMethod.POST](null!, null!));


        Assert.Throws<RetrievedException>(() => router.lookUp(route2)!.handlers![HttpMethod.GET](null!, null!));
        Assert.Throws<CreatedException>(() => router.lookUp(route2)!.handlers![HttpMethod.POST](null!, null!));

        Assert.Throws<CreatedException>(() => router.lookUp(route3)!.handlers![HttpMethod.POST](null!, null!));
        Assert.Throws<RetrievedException>(() => router.lookUp(route3)!.handlers![HttpMethod.GET](null!, null!));
    }

    [Fact]
    public void shouldRouteAndNotFindRoute()
    {
        Router router = new Router();

        String route1 = "/path/to/resource";
        router.route(HttpMethod.GET, route1, (_, _) =>
        {
            throw new RetrievedException();
        });
        Assert.Throws<RetrievedException>(() => router.lookUp(route1)!.handlers![HttpMethod.GET](null!, null!));

        Assert.False(router.lookUp(route1)!.handlers!.ContainsKey(HttpMethod.POST));

        String route2 = "/path/to/another/resource";
        Assert.Null(router.lookUp(route2));
    }
}
