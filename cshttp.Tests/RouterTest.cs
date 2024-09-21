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


        Assert.Throws<RetrievedException>(() => router.lookUp(HttpMethod.GET, route1)!(null!, null!));
        Assert.Throws<CreatedException>(() => router.lookUp(HttpMethod.POST, route1)!(null!, null!));


        Assert.Throws<RetrievedException>(() => router.lookUp(HttpMethod.GET, route2)!(null!, null!));
        Assert.Throws<CreatedException>(() => router.lookUp(HttpMethod.POST, route2)!(null!, null!));

        Assert.Throws<CreatedException>(() => router.lookUp(HttpMethod.POST, route3)!(null!, null!));
        Assert.Throws<RetrievedException>(() => router.lookUp(HttpMethod.GET, route3)!(null!, null!));
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
        Assert.Throws<RetrievedException>(() => router.lookUp(HttpMethod.GET, route1)!(null!, null!));

        Assert.Null(router.lookUp(HttpMethod.POST, route1));

        String route2 = "/path/to/another/resource";
        Assert.Null(router.lookUp(HttpMethod.GET, route2));
    }
}
