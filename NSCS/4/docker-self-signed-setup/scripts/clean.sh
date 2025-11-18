#!/bin/bash

echo "Lösche generierte Zertifikate und Keys..."
rm -f certs/*.key certs/*.crt certs/*.csr certs/*.srl certs/*.pem
echo "Fertig!"
