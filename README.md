# Kubernetes Setup for Certificate Service with gRPC, HTTP Communication, and Asynchronous Messaging

This repository contains Kubernetes configuration files for deploying and exposing services such as `certificateservice`, `skillservice`, and `RabbitMQ`, alongside other configurations like `ingress` and `mssql`. Below are the instructions on how to deploy these configurations in a Kubernetes cluster.

## Overview

The following Kubernetes YAML files are provided:

- `certificates-depl.yaml`: Contains the Deployment configuration for the `certificateservice` with both HTTP and gRPC endpoints.
- `certificates-clusterip-srv.yaml`: Contains the ClusterIP Service configuration for internal communication, exposing both HTTP and gRPC ports for `certificateservice`.
- `certificate-np-srv.yaml`: Exposes the `certificateservice` externally via a NodePort service for HTTP communication.
- `rabbitmq-depl.yaml`: Deployment and Service configuration for RabbitMQ, used for asynchronous communication between microservices.
- `skillservice-depl.yaml`: Contains the Deployment and Service configuration for **SkillService**.
- `ingress-srv.yaml`: Configures Ingress to route traffic to the services using NGINX as a load balancer.
- `mssql-depl.yaml`: Contains the Deployment and Service configuration for SQL Server, which serves as the database backend for **CertificateService**.

## Prerequisites

1. **Kubernetes Cluster**: Ensure you have a Kubernetes cluster running. You can use Minikube, kubeadm, or any managed Kubernetes service (e.g., Google Kubernetes Engine, Amazon EKS, Azure Kubernetes Service).

2. **kubectl**: Ensure you have `kubectl` installed and configured to interact with your Kubernetes cluster.

## Instructions

### 1. Apply All Kubernetes Configurations
with below command 
- kubectl apply -f .
Navigate to the folder where your YAML files are located.

```bash
cd /path/to/CertificateHub/k8s
