# SoapService-MixEncoding

This was a use case when translating a Java service into a .Net. The java implementation has this behavior and the new dotnet implementation needed to do the same.

## Presentation

When reading the body stream of the message éÃ©é the equivalent in hex is E9 C3 A9 E9 where E9 is the unicode for é and C3 A9 is the UTF-8 for é. By checking two range, both encoding can be use when it's appropriate.

The Program.cs file contain the for loop and the if else necessary.

## Note on the postman test

See the postman collection to look at what happend to every character from 00 to FF and every utf-8 from 21 to C3 BF where the critical range is C2 80 to C3 BF.
