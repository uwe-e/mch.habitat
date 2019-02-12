module.exports = function () {
    var instanceRoot = "d:\\Work\\Sitecore\\mch.habitat.v8.dev";
    var config = {
        websiteRoot: instanceRoot + "\\Website",
        sitecoreLibraries: instanceRoot + "\\Website\\bin",
        licensePath: instanceRoot + "\\Data\\license.xml",
        solutionName: "mch.habitat",
        buildConfiguration: "Debug",
        buildPlatform: "Any CPU",
        buildToolsVersion: 15.0, //change to 14.0 for VS2015 support
        publishPlatform: "AnyCpu",
        runCleanBuilds: false
    };
    return config;
}
