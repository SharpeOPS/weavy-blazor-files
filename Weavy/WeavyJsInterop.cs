using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorApp.Weavy {
    public class WeavyJsInterop : IDisposable {
        private readonly IJSRuntime JS;
        private bool Initialized = false;
        private IJSObjectReference Bridge;
        private ValueTask<IJSObjectReference> WhenImport;

        // Constructor
        // This is a good place to inject any authentication service you may use to provide JWT tokens.
        public WeavyJsInterop(IJSRuntime js) {
            JS = js;
        }

        // Initialization of the JS Interop Module
        // The initialization is only done once even if you call it multiple times
        public async Task Init() {
            if (!Initialized) {
                Initialized = true;
                WhenImport = JS.InvokeAsync<IJSObjectReference>("import", "./weavyJsInterop.js");
                Bridge = await WhenImport;
            } else {
                await WhenImport;
            }
        }

        // Calling Javascript to create a new instance of Weavy via the JS Interop Module
        public async ValueTask<IJSObjectReference> Weavy(object options = null) {
            await Init();
            // Demo JWT only for showcase.weavycloud.com
            // Configure your JWT here when using your own weavy server
            var jwt = new { jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0ZXN0IiwiaWF0IjoxNjA1MDM1MTY2LCJleHAiOjE2MzY1NzExNzYsInN1YiI6IlRlc3QyIiwiY2xpZW50X2lkIjoiY2xpZW50aWQiLCJ1c2VybmFtZSI6IlRlc3QyIiwiZW1haWwiOiJ0ZXN0MkBlbWFpbC5jb20iLCJkaXIiOiJ3ZWF2eSJ9.1GZLZS0KEXyXGLZM1EOEgYIZYu2Gf2doyPY0trha_eI" };
            return await Bridge.InvokeAsync<IJSObjectReference>("weavy", new object[] { jwt, options });
        }

        public void Dispose() {
            Bridge?.DisposeAsync();
        }
    }
}
