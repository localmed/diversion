Using the Diversion msbuild task:

By default, the diversion msbuild will cause the build to fail if the version number specified in the project is not at least the version that it calculates.

To have diversion override the version specified if it is lower than the version it calculates, create a diversion.json config file as shown below in your project root.

{
    "IsCorrectionEnabled": true,
    "GenerateDiversionFile": false
}

If you'd like to diversion to generate a diversion.output.json file into your output folder showing the differences detected, change "GenerateDiversionFile" to true.

