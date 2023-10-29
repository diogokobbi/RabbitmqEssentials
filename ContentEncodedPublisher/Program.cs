using System.Runtime.CompilerServices;
using System.Text;
using BrotliSharpLib;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "compressed-messages",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var lipsumMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Amet consectetur adipiscing elit ut aliquam purus sit amet. Iaculis nunc sed augue lacus viverra vitae. Aliquam id diam maecenas ultricies mi eget mauris. Ultrices in iaculis nunc sed augue lacus viverra vitae congue. Purus ut faucibus pulvinar elementum integer enim neque volutpat. Justo eget magna fermentum iaculis eu non. Ac tincidunt vitae semper quis lectus nulla at volutpat. Massa tincidunt dui ut ornare lectus sit amet est. Magna eget est lorem ipsum dolor sit amet consectetur. Vel turpis nunc eget lorem dolor sed viverra. Sit amet est placerat in egestas erat imperdiet sed euismod. Elit eget gravida cum sociis. Venenatis urna cursus eget nunc scelerisque. Ut faucibus pulvinar elementum integer enim neque volutpat ac. Mi bibendum neque egestas congue quisque egestas diam. Ut porttitor leo a diam sollicitudin tempor id.\r\n\r\nSit amet luctus venenatis lectus magna fringilla urna porttitor rhoncus. Commodo quis imperdiet massa tincidunt nunc pulvinar sapien et ligula. Eget mauris pharetra et ultrices neque. Eros donec ac odio tempor orci dapibus ultrices. Consequat id porta nibh venenatis cras sed. Ullamcorper sit amet risus nullam eget felis eget. Imperdiet nulla malesuada pellentesque elit eget gravida cum sociis natoque. Porttitor leo a diam sollicitudin tempor id eu nisl. Purus ut faucibus pulvinar elementum integer enim neque volutpat ac. Mauris cursus mattis molestie a iaculis at erat pellentesque adipiscing. Massa tempor nec feugiat nisl pretium fusce id velit.\r\n\r\nArcu cursus euismod quis viverra nibh cras pulvinar mattis nunc. Id eu nisl nunc mi ipsum faucibus. Lacus laoreet non curabitur gravida. Fringilla ut morbi tincidunt augue interdum. Habitant morbi tristique senectus et netus. Amet mauris commodo quis imperdiet massa. Scelerisque varius morbi enim nunc. Odio euismod lacinia at quis risus sed vulputate. Quis imperdiet massa tincidunt nunc pulvinar sapien et ligula. Pellentesque diam volutpat commodo sed.\r\n\r\nAc orci phasellus egestas tellus rutrum tellus pellentesque eu tincidunt. Eu non diam phasellus vestibulum lorem sed risus. Id consectetur purus ut faucibus pulvinar elementum integer. Mi in nulla posuere sollicitudin aliquam ultrices sagittis. Tempus quam pellentesque nec nam aliquam. Non quam lacus suspendisse faucibus interdum posuere lorem. Faucibus et molestie ac feugiat sed lectus vestibulum mattis. Non blandit massa enim nec dui nunc mattis. Laoreet sit amet cursus sit amet dictum sit amet. At tempor commodo ullamcorper a lacus. Porta lorem mollis aliquam ut porttitor leo a diam sollicitudin. Sit amet nulla facilisi morbi. Ut placerat orci nulla pellentesque dignissim.\r\n\r\nSed pulvinar proin gravida hendrerit lectus a. Aliquam faucibus purus in massa tempor. Ac orci phasellus egestas tellus rutrum tellus pellentesque eu tincidunt. Est placerat in egestas erat imperdiet. Sed tempus urna et pharetra pharetra massa. Morbi leo urna molestie at. Pulvinar mattis nunc sed blandit libero volutpat sed. Vel facilisis volutpat est velit egestas dui id ornare. Ut porttitor leo a diam sollicitudin tempor id eu nisl. Viverra justo nec ultrices dui sapien eget mi. Odio euismod lacinia at quis risus sed vulputate. Tincidunt arcu non sodales neque sodales ut etiam sit. Cursus vitae congue mauris rhoncus aenean vel elit scelerisque. Morbi tristique senectus et netus. Et netus et malesuada fames ac. Molestie ac feugiat sed lectus vestibulum mattis ullamcorper velit sed. Ultricies lacus sed turpis tincidunt id. Tristique senectus et netus et malesuada. Arcu odio ut sem nulla pharetra diam sit amet nisl.";

var lipsumMessagedByteArray = Encoding.UTF8.GetBytes(lipsumMessage);

channel.BasicPublish(exchange: string.Empty,
                     routingKey: "compressed-messages",
                     basicProperties: null,
                     body: lipsumMessagedByteArray);

var basicProperties = channel.CreateBasicProperties();
basicProperties.ContentEncoding = "gzip";

var uncompressedBody = Encoding.UTF8.GetBytes(lipsumMessage);
var compressedBody = Brotli.CompressBuffer(uncompressedBody, 0, uncompressedBody.Length);
channel.BasicPublish(exchange: string.Empty,
                        routingKey: "compressed-messages",
                        basicProperties: basicProperties,
                        body: compressedBody);



