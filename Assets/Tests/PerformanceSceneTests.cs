using System.Collections;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    /// <summary>
    /// Simple test that runs a scene and takes frame time measurements.
    /// </summary>
    public class PerformanceSceneTests
    {
        private readonly string performanceSceneName = "Performance";

        /// <summary>
        /// Helper class that wraps a few arguments.
        /// </summary>
        public sealed class TestParameters
        {
            public TestParameters(float timeToSpawnBody = 1f, int numberOfBodiesToSpawnAtOnce = 1, string currentSampleGroup = default, int numberOfMeasureMents = 500)
            {
                this.timeToSpawnBody = timeToSpawnBody;
                this.numberOfBodiesToSpawnAtOnce = numberOfBodiesToSpawnAtOnce;
                this.numberOfMeasureMents = numberOfMeasureMents;
                this.currentSampleGroup = currentSampleGroup;
            }

            public float timeToSpawnBody { get; }
            public int numberOfBodiesToSpawnAtOnce { get; }
            public int numberOfMeasureMents { get; }
            public string currentSampleGroup { get; set; }

        }

        // Overall test parameters.
        private const int bodyIterations = 3;
        private const int timeIterations = 5;
        private const int timeMultiplier = 5;
        private const int initialPowerOfTwo = 1 << 2;

        [Performance, UnityTest]
        public IEnumerator TestPerformance_1Sec()

        {
            var parameters = new TestParameters(1f);
            return RunTest(parameters);
        }

        [Performance, UnityTest]
        public IEnumerator TestPerformance_0001Sec()
        {
            var parameters = new TestParameters(0.001f);
            return RunTest(parameters);
        }

        [Performance, UnityTest]
        public IEnumerator TestPerformance_01Sec_2Bodies()
        {
            var parameters = new TestParameters(0.1f, 2);
            return RunTest(parameters);
        }

        [Performance, UnityTest]
        public IEnumerator TestPerformance_01Sec_32Bodies()
        {
            var parameters = new TestParameters(0.1f, 32);
            return RunTest(parameters);
        }

        /// <summary>
        /// Composite test that tests a few different scenarios, decreasing time between spawns and increasing the number of bodies spawned.
        /// </summary>
        [Performance, UnityTest]
        public IEnumerator CompositeTest()
        {
            for (int bodyPower = 0; bodyPower < bodyIterations; bodyPower++)
            {
                int divisor = 1;
                for (int timePower = 0; timePower < timeIterations; timePower++)
                {
                    float time = 1f / divisor;
                    divisor *= timeMultiplier;
                    int bodies = initialPowerOfTwo << bodyPower;
                    var parameters = new TestParameters(time, bodies, $"Composite_T{time}_B{bodies}");
                    yield return RunTest(parameters);
                }
            }
        }

        /// <summary>
        /// Starts running a test.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerator RunTest(TestParameters parameters)
        {
            yield return LoadScene(parameters);
            if (string.IsNullOrEmpty(parameters.currentSampleGroup))
            {
                yield return Measure
                    .Frames()
                    .MeasurementCount(parameters.numberOfMeasureMents)
                    .Run();
            }
            else
            {
                yield return Measure
                    .Frames()
                    .MeasurementCount(parameters.numberOfMeasureMents)
                    .SampleGroup(parameters.currentSampleGroup)
                    .Run();
            }
        }

        /// <summary>
        /// Sets up the physics cube spawner in the scene.
        /// </summary>
        private void SetupSpawner(TestParameters parameters, SpawnCubes spawner)
        {
            spawner.timeToSpawnBody = parameters.timeToSpawnBody;
            spawner.numberOfBodiesToSpawnAtOnce = parameters.numberOfBodiesToSpawnAtOnce;
        }

        /// <summary>
        /// Loads a scene and sets it up for testing.
        /// </summary>
        private IEnumerator LoadScene(TestParameters parameters)
        {
            yield return SceneManager.LoadSceneAsync(performanceSceneName);
            var performanceScene = SceneManager.GetSceneByName(performanceSceneName);
            SceneManager.SetActiveScene(performanceScene);
            foreach (var rootObject in performanceScene.GetRootGameObjects())
            {
                var spawner = rootObject.GetComponent<SpawnCubes>();
                if (spawner != null)
                {
                    SetupSpawner(parameters, spawner);
                    break;
                }
            }
        }
    }
}