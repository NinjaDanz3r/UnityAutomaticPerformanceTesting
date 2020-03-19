# UnityAutomaticPerformanceTesting
A simple little Unity project that tests if spawning a lot of Rigidbody cubes in a short time span reduces FPS.
Utilizes the new performance testing framework in order to take measurements and present them.

## How to run
Select Window->General->Test Runner, click "PlayMode" and then hit the "Run all" button, or the "Run all in player button".
After the tests have finished running, select Window->Analysis->Performance test report and hit the "Refresh" button in order to see the results.

To run the tests through a console, use the following console command:
Unity.exe -projectPath [Path to project directory] -testPlatform [your platform here] -buildTarget [your target here] -runTests -batchmode
After the tests have run, a magical little .xml report will appear in your project root that can be read in the editor the same way as above.

## Helpful links
https://docs.unity3d.com/Packages/com.unity.test-framework.performance@1.0/manual/index.html
https://docs.unity3d.com/Packages/com.unity.test-framework@1.0/manual/
https://blogs.unity3d.com/2018/09/25/performance-benchmarking-in-unity-how-to-get-started/
