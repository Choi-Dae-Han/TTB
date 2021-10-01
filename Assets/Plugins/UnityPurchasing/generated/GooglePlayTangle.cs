#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("7SaK1AGY3By3YH+tA53aB35HQtxvC48fVgy6P7mLSUr8H9es0cV16VH7uk7w2QkaBQDMdGwOJvzvfvV5AI8nDPtDD65uHZ0tM2Cl7DfRqXXgBMveh/tRUA01MkWy+dtnmwlRc4O3CFn3/f0UOUSW9zpeFSqr2lvZB3CdA41+0hHP/rT94ttbflqYjbZWHI8w4DTn3bfb/I/T/U7Br+qGVafkqFQjGk5U3Vh6MeJjKEg5b9NWo4f8kw2K2IqPe5Ivx091kbQEEN46o0rI34pIdZU6hskeSE6Qfuwgng2OgI+/DY6FjQ2Ojo8Yc+/hfP4cvw2Orb+CiYalCccJeIKOjo6Kj4wfYzSnDCBSVUymFWDfxyGzfZ/Sl7neOPO/7N/WdI2Mjo+O");
        private static int[] order = new int[] { 3,13,10,8,5,11,11,10,10,12,11,13,13,13,14 };
        private static int key = 143;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
