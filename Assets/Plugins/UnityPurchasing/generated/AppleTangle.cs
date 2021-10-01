#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("BTMMBQBWHhACAvwHBjMAAgL8Mx53a2xxand6MhUzFwUAVgcAEA5Cc3dqZWpgYndmI2F6I2JteiNzYnF3lp15D6dEiFjXFTQwyMcMTs0XatIlMycFAFYHCBAeQnNzb2YjQGZxd2FvZiNwd2JtZ2JxZyN3ZnFucCNiI0BCM4ECITMOBQophUuF9A4CAgIHBRABVlAyEDMSBQBWBwkQCUJzcxyS2B1EU+gG7l16hy7oNaFUT1bvMDVZM2EyCDMKBQBWBwUQAVZQMhAchoCGGJo+RDTxqphDjS/XspMR2wtdM4ECEgUAVh4jB4ECCzOBAgcztjmu9wwNA5EIsiIVLXfWPw7YYRWooHKRRFBWwqwsQrD7+OBzzuWgT3ojYnBwdm5mcCNiYGBmc3dibWBmb2YjSm1gLTIlMycFAFYHCBAeQnMtQ6X0RE58C10zHAUAVh4gBxszFTYxMjczMDVZFA4wNjMxMzoxMjczgxco02pEl3UK/fdoji1DpfRETnxapAYKfxRDVRIdd9C0iCA4RKDWbGc2IBZIFloesJf09Z+dzFO5wltTDJ4+8ChKKxnL/c22ug3aXR/VyD5zb2YjUWxsdyNAQjMdFA4zNTM3McoacfZeDdZ8XJjxJgC5VoxOXg7yq999ITbJJtbaDNVo16EnIBL0oq8phUuF9A4CAgYGAzNhMggzCgUAVnkzgQJ1Mw0FAFYeDAIC/AcHAAECLDOCwAULKAUCBgYEAQEzgrUZgrA1mk8ue7Tuj5jf8HSY8XXRdDNMwj4lZCOJMGn0DoHM3eigLPpQaVhnUWZvamJtYGYjbG0jd2tqcCNgZnG993CY7dFnDMh6TDfboT36e/xoy21nI2BsbWdqd2psbXAjbGUjdnBmc29mI0BmcXdqZWpgYndqbG0jQnZxYmB3amBmI3B3YndmbmZtd3AtMyNibWcjYGZxd2plamBid2psbSNzfEKrm/rSyWWfJ2gS06C45xgpwBwE7346gIhQI9A7x7K8mUwJaPwo/4ECAwUKKYVLhfRgZwYCM4LxMykFM4EHuDOBAKCjAAECAQECATMOBQpK23WcMBdmonSXyi4BAAIDAqCBArQYvpBBJxEpxAwetU6fXWDLSIMUw2AwdPQ5BC9V6NkMIg3ZuXAaTLZkjAu3I/TIry8jbHO1PAIzj7RAzAsoBQIGBgQBAhUda3d3c3A5LCx0J+Ho0rRz3AxG4iTJ8m577uS2FBSyM1vvWQcxj2uwjB7dZnD8ZF1mvwYDAIECDAMzgQIJAYECAgPnkqoKBQBWHg0HFQcXKNNqRJd1Cv33aI50dC1ic3NvZi1gbG4sYnNzb2ZgYjMSBQBWBwkQCUJzc29mI0ptYC0yLyNgZnF3amVqYGJ3ZiNzbG9qYHoVMxcFAFYHABAOQnNzb2YjUWxsd2plamBid2psbSNCdndrbHFqd3oyiBqK3fpIb/YEqCEzAesbPftTCtBGfRxPaFOVQorHd2EIE4BChDCJgg4FCimFS4X0DgICBgYDAIECAgNfI2xlI3drZiN3a2ZtI2Jzc29qYGLaNXzChFbapJq6MUH429ZynX2iUYxwgmPFGFgKLJGx+0dL82M7nRb2U6mJ1tnn/9MKBDSzdnYi");
        private static int[] order = new int[] { 29,5,39,18,10,42,13,39,15,15,49,33,23,24,40,18,31,39,32,52,20,22,24,52,54,42,28,37,48,56,38,50,43,49,46,45,39,51,46,56,51,59,57,54,45,53,48,51,48,56,54,53,52,56,54,57,56,59,58,59,60 };
        private static int key = 3;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
