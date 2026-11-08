QT += widgets

CONFIG += c++17

TARGET = XmlExplorer
TEMPLATE = app

# statisch gelinkt: keine Import-Symbole noetig

INCLUDEPATH += $$PWD/../tinyxml2lib
LIBS += -L$$OUT_PWD/../tinyxml2lib -ltinyxml2

DEPENDPATH += $$PWD/../tinyxml2lib

SOURCES += \
    main.cpp \
    mainwindow.cpp

HEADERS += \
    mainwindow.h

FORMS += \
    mainwindow.ui
