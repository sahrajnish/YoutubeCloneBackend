# YoutubeCloneBackend

A learning-focused backend for a YouTube-clone project implementing microservices and clean architecture using .NET 8.

## Table of Contents

- [Project Overview](#project-overview)
- [Core Services](#core-services)
- [Supporting Layers](#supporting-layers)
- [Technology Stack](#technology-stack)

## Project Overview

This repository contains the backend codebase for a YouTube-clone application, designed for learning microservices, clean architecture, and .NET development. The project showcases separation of concerns, best practices, and is a work-in-progress for educational growth.

## Core Services

- **APIGateway:** Central entry-point for routing API requests to microservices.
- **UserServices API:** Handles user management — registration.

## Supporting Layers

- **Core:** Business models and shared logic across services.
- **Persistence:** Database integration and repository patterns for data access.
- **Services:** Service-layer orchestrating business rules and communication between APIs.

## Technology Stack

| Component            | Details                            |
|----------------------|------------------------------------|
| Framework            | .NET 8.0                           |
| Architecture         | Microservices, Clean Architecture  |
| API Documentation    | Swagger (OpenAPI)                  |
| Database             | PostgreSQL (SQL)                   |
| ORM/Data Access      | Npgsql, Dapper  |
