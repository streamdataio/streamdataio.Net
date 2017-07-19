# Streamdataio-DotNet

## About

A .Net demo for Streamdata.io service

Streamdata.io is a proxy service that queries your REST APIs from one side and sends updates to clients in real time with JsonPatch.

The service relies only on two main standards [SSE (Server Sent Events)](https://en.wikipedia.org/wiki/Server-sent_events) and [JsonPatch](https://tools.ietf.org/html/rfc6902). 
That means that you can connect to the service with your own tools and methods. This demo application shows to use Streamdata.io service.

Streamdata.io is available in SaaS mode, with a free plan that enables you to test the service. To do so, you just need to create an account on https://portal.streamdata.io/#/register

In order to test the service, Streamdata.io provides a sample REST API with frequent updates that you can use to experiment with the service: [http://stockmarket.streamdata.io/prices](http://stockmarket.streamdata.io/prices).

## Run the demo

Clone the repo and edit Program.cs file:
 * Update this line to set your Streamdata.io token
	var token = "<YOUR STREAMDATAIO TOKEN>";
 * Update this line if you want to test with a different URL
	var url = "https://streamdata.motwin.net/http://stockmarket.streamdata.io/prices" + "?X-Sd-Token=" + token;
 * Run project in Visual Studio. 

## Conclusion

Please feel free to contribute, the service is amazingly simple and efficient but we can still find ways to improve the client to have an even better developer experience ;-).

Happy coding!


