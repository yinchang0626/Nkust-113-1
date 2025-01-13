plugins {
    kotlin("jvm") version "2.0.21"
    application
}

group = "com.example.population"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    // Spark Java HTTP server library
    implementation("com.sparkjava:spark-core:2.9.4")

    // Gson for JSON serialization
    implementation("com.google.code.gson:gson:2.8.9")

    // SQLite JDBC Driver
    implementation("org.xerial:sqlite-jdbc:3.43.2.1")

    // Kotlin testing library
    testImplementation(kotlin("test"))
}

application {
    // 設定主程式入口
    mainClass.set("com.example.population.ApplicationKt")
}

kotlin {
    jvmToolchain(21)
}

tasks.test {
    useJUnitPlatform()
}
