#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <cmath>
#include <exception>
#include <limits>
#include <memory>
#include <stdexcept>
#include <variant>
#include <QLocale>
#include <QMessageBox>
#include "sensor.h"
#include "messwertstatistik.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    ui->tableWidget->horizontalHeader()
        ->setSectionResizeMode(QHeaderView::Stretch);

    auto* temperatur = new QTreeWidgetItem(ui->treeWidget, {tr("Temperatur")});
    auto* distanz = new QTreeWidgetItem(ui->treeWidget, {tr("Distanz")});

    temperaturMaximum = new QTreeWidgetItem(temperatur, {tr("Keine Messwerte")});
    distanzMaximum = new QTreeWidgetItem(distanz, {tr("Keine Messwerte")});

    ui->treeWidget->expandAll();
}

MainWindow::~MainWindow()
{
    delete ui;
}

static std::variant<float, double> liesMesswert(QString input)
{
    bool ok = false;
    const double wert = input.replace(',', '.').toDouble(&ok);

    if (!ok && std::isinf(wert)) {
        throw std::overflow_error("Die Zahl ist zu groß für double.");
    }
    if (!ok || !std::isfinite(wert)) {
        throw std::domain_error("Keine gültige Zahl!");
    }

    if (std::abs(wert) > std::numeric_limits<float>::max()) {
        return wert;
    }
    return static_cast<float>(wert);
}

void MainWindow::on_pushButton_clicked()
{
    try {
        const auto zahl = liesMesswert(ui->lineEdit->text());
        const QString messart = ui->comboBox->currentText();
        if (const auto* wert = std::get_if<float>(&zahl)) {
            newElement(messart, *wert, QStringLiteral("float"));
        } else {
            newElement(messart, std::get<double>(zahl), QStringLiteral("double"));
        }
    } catch (const std::domain_error &fehler) {
        QMessageBox::warning(this, "Rechenfehler", QString::fromUtf8(fehler.what()));
    } catch (const std::overflow_error &fehler) {
        QMessageBox::warning(this, "Zahl zu groß", QString::fromUtf8(fehler.what()));
    } catch (const std::exception &fehler) {
        QMessageBox::critical(this, "Fehler", QString::fromUtf8(fehler.what()));
    }
}

void MainWindow::newElement(const QString &messart, double wert, const QString &datentyp)
{
    const bool istTemperatur = ui->comboBox->currentIndex() == 0;
    const TemperaturSensor temperatur;
    const DistanzSensor distanz;
    const Sensor& sensor = istTemperatur
        ? static_cast<const Sensor&>(temperatur)
        : static_cast<const Sensor&>(distanz);
    const QString einheit = sensor.einheit();

    auto& werte = istTemperatur ? tempList : distList;
    auto* maximumEintrag = istTemperatur ? temperaturMaximum : distanzMaximum;
    werte.push_back(wert);

    MesswertStatistik ms;
    const auto maximum = std::make_shared<const double*>(
        ms.findeMaximum(werte.data(), static_cast<int>(werte.size()))
    );
    if (maximum && *maximum) {
        const QString maximumText = QString::number(**maximum, 'g', QLocale::FloatingPointShortest);
        maximumEintrag->setText(0, tr("Maximum %1 %2").arg(maximumText, einheit));
    }

    auto neu_1 = new QTableWidgetItem(messart);
    auto neu_2 = new QTableWidgetItem(QString::number(wert, 'g', QLocale::FloatingPointShortest));
    auto neu_3 = new QTableWidgetItem(einheit);
    auto neu_4 = new QTableWidgetItem(datentyp);

    auto* tabelle = ui->tableWidget;
    const int zeile = tabelle->rowCount();

    tabelle->insertRow(zeile);

    ui->tableWidget->setItem(zeile, 0, neu_1);
    ui->tableWidget->setItem(zeile, 1, neu_2);
    ui->tableWidget->setItem(zeile, 2, neu_3);
    ui->tableWidget->setItem(zeile, 3, neu_4);


}

