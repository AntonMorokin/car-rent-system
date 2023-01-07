docker exec -i kafka /opt/bitnami/kafka/bin/kafka-topics.sh --create --topic clients --bootstrap-server localhost:9092
docker exec -i kafka /opt/bitnami/kafka/bin/kafka-topics.sh --create --topic cars --bootstrap-server localhost:9092
docker exec -i kafka /opt/bitnami/kafka/bin/kafka-topics.sh --create --topic rides --bootstrap-server localhost:9092