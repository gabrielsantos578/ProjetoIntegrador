import { useState } from 'react';
import PersonClass from '../person';

function SupervisorClass() {
  const person = PersonClass();

  // Atributos
  const personPicture = person.personPicture;
  const personName = person.personName;
  const setPersonName = person.setPersonName;
  const personEmail = person.personEmail;
  const setPersonEmail = person.setPersonEmail;
  const personTelephone = person.personTelephone;
  const setPersonTelephone = person.setPersonTelephone;
  const personCpfCnpj = person.personCpfCnpj;
  const setPersonCpfCnpj = person.setPersonCpfCnpj;
  const personRgIe = person.personRgIe;
  const setPersonRgIe = person.setPersonRgIe;

  // Erros
  const errorPersonPicture = person.errorPersonPicture;
  const errorPersonName = person.errorPersonName;
  const errorPersonEmail = person.errorPersonEmail;
  const errorPersonTelephone = person.errorPersonTelephone;
  const errorPersonCpfCnpj = person.errorPersonCpfCnpj;
  const errorPersonRgIe = person.errorPersonRgIe;

  // Funções Essenciais
  const getDataPerson = person.getData;
  const setDataPerson = person.setData;
  const getErrorPerson = person.getError;
  const clearDataPerson = person.clearData;
  const clearErrorPerson = person.clearError;
  const verifyDataPerson = person.verifyData;

  // Variáveis e Funções de Controle
  const identifyCpfCnpj = person.identifyCpfCnpj;
  const setIdentifyCpfCnpj = person.setIdentifyCpfCnpj;
  const identifyRgIe = person.identifyRgIe;
  const setIdentifyRgIe = person.setIdentifyRgIe;
  const handlePhone = person.handlePhone;
  const handleCpfCnpj = person.handleCpfCnpj;
  const handleRgIe = person.handleRgIe;
  const closeIcon = person.closeIcon;
  const addImage = person.addImage;
  const insertPicture = person.insertPicture;
  const removePicture = person.removePicture;
  const handleImageClick = person.handleImageClick;
  person.effects();

  const [supervisorId, setSupervisorId] = useState(0);

  function propertyName() {
    return "Fiscal " + personName;
  }

  function gender() {
    return "o";
  }

  function getData() {
    return {
      ...getDataPerson(),
      id: supervisorId,
    }
  }

  function setData(object) {
    setDataPerson(object);
    setSupervisorId(object.id);
  }

  function getError(json) {
    getErrorPerson(json);
  }

  function clearData() {
    clearDataPerson();
    setSupervisorId(0);
  }

  function clearError() {
    clearErrorPerson();
  }

  function verifyData() {
    clearError();
    return verifyDataPerson();
  }


  return {
    /* -----  Pessoa  ----- */

    // Atributos
    personPicture,
    personName,
    setPersonName,
    personEmail,
    setPersonEmail,
    personTelephone,
    setPersonTelephone,
    personCpfCnpj,
    setPersonCpfCnpj,
    personRgIe,
    setPersonRgIe,

    // Erros
    errorPersonPicture,
    errorPersonName,
    errorPersonEmail,
    errorPersonTelephone,
    errorPersonCpfCnpj,
    errorPersonRgIe,

    // Variáveis e Funções de Controle
    identifyCpfCnpj,
    setIdentifyCpfCnpj,
    identifyRgIe,
    setIdentifyRgIe,
    handlePhone,
    handleCpfCnpj,
    handleRgIe,
    closeIcon,
    addImage,
    insertPicture,
    removePicture,
    handleImageClick,


    /* -----  Engenheiro  ----- */

    // Atributos
    supervisorId,
    setSupervisorId,

    // Funções Essencias
    propertyName,
    gender,
    getData,
    setData,
    getError,
    clearData,
    clearError,
    verifyData,
  };
}

export default SupervisorClass;