module.exports = function () {
    var instanceRoot = "D:\\Work\\Sitecore\\mch.habitat.dev.local";
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
