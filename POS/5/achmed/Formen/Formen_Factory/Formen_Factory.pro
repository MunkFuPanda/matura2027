# Formen-Factory: Bibliothek mit Basisklasse Form und Factory.
QT += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TEMPLATE = lib
DEFINES += FORMEN_LIBRARY

CONFIG += c++17

SOURCES += \
    factory.cpp \
    form.cpp

HEADERS += \
    factory.h \
    form.h

# DLL + Import-Lib ins Build-Verzeichnis der App legen, damit
# Formen.exe die Bibliothek zur Laufzeit findet.
DESTDIR = $$OUT_PWD/../Formen
