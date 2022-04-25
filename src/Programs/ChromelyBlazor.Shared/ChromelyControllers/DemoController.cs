// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable CA1822

namespace ChromelyBlazor.Shared.ChromelyControllers;

/// <summary>
/// The movie controller.
/// </summary>
[ChromelyController(Name = "DemoController")]
public class DemoController : ChromelyController
{
    private readonly IChromelyConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="DemoController"/> class.
    /// </summary>
    public DemoController(IChromelyConfiguration config)
    {
        _config = config;
    }


    [ChromelyRoute(Path = "/democontroller/showdevtools")]
    public void ShowDevTools()
    {
        if (_config != null && !string.IsNullOrWhiteSpace(_config.DevToolsUrl))
        {
            BrowserLauncher.Open(_config.Platform, _config.DevToolsUrl);
        }
    }

}



