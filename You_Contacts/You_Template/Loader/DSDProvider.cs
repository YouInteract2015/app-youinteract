using You_Contacts.DSDWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace You_Contacts.Loader
{
    class DSDProviderLOADER : IDisposable  // release allocated resources
    {
        //lets you specify a block of code that you can expand or collapse
        #region Fields;
        // Reference to the DSD WebServices
        private You_Contacts.DSDWS.WSSoap _DSD;

        // Check if the class has been disposed of.
        private bool _disposed = false;

        // Department ID
        public int IDDept { get; private set; }

        // Semester ID
        public int IDSem { get; private set; }

        // Path to content on the DSD Server.
        public const string DSDFilePath = "http://dsd.av.it.pt/App_Upload/";

        #endregion

        #region Constructor and Destructor
        // Constructor
        public DSDProviderLOADER()
        {
            // Set up callback to ignore certificate validation
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

            // Initialize DSD WS client
            _DSD = new WSSoapClient();

            // Set up Department and Semester IDs
            IDDept = 1;
            IDSem = 6;
        }

        // Destructor
        ~DSDProviderLOADER()
        {
            Dispose(false);
        }
        #endregion

        #region Web Service Consumption
        // Request a list of teachers
        public List<Docente> GetDocentes(int dept)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getDocentesRequestBody docentesBody = new getDocentesRequestBody(dept, 0);
                getDocentesRequest docentesReq = new getDocentesRequest(docentesBody);

                // get response
                getDocentesResponse resp = _DSD.getDocentes(docentesReq);

                // return list
                return resp.Body.getDocentesResult.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Requests a list of departments.
        /// </summary>
        /// <returns>List of Departments</returns>
        public List<Departamento> GetDepartamentos(int rev)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getDepartamentosRequestBody DepartamentosBody = new getDepartamentosRequestBody(rev);
                getDepartamentosRequest DepartamentosReq = new getDepartamentosRequest(DepartamentosBody);

                // get response
                getDepartamentosResponse resp = _DSD.getDepartamentos(DepartamentosReq);

                // return lists
                return resp.Body.getDepartamentosResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Requests a list of Courses 
        /// </summary>
        /// <param name="dept">Department no.</param>
        /// <returns>List of Courses</returns>
        public List<Curso> GetCursos(int dept)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getCursosRequestBody CursosBody = new getCursosRequestBody(dept, 0);
                getCursosRequest CursosReq = new getCursosRequest(CursosBody);

                // get response
                getCursosResponse resp = _DSD.getCursos(CursosReq);

                // return lists
                return resp.Body.getCursosResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Requests a list of Subjects
        /// </summary>
        /// <param name="dept">Department no.</param>
        /// <returns>List of Subjects</returns>
        public List<Disciplina> GetDisciplinas(int dept)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getDisciplinasRequestBody DisciplinasBody = new getDisciplinasRequestBody(dept, 0);
                getDisciplinasRequest DisciplinasReq = new getDisciplinasRequest(DisciplinasBody);

                // get response
                getDisciplinasResponse resp = _DSD.getDisciplinas(DisciplinasReq);

                // return lists
                return resp.Body.getDisciplinasResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Requests a list of Semesters
        /// </summary>
        /// <returns>List of Semester</returns>
        public List<Semestre> GetSemestres()
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                getSemestresRequestBody bodysemestres = new getSemestresRequestBody(0);
                getSemestresRequest reqsemestres = new getSemestresRequest(bodysemestres);

                getSemestresResponse resp = _DSD.getSemestres(reqsemestres);

                List<Semestre> semestres = resp.Body.getSemestresResult.ToList();

                return semestres;
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString()); 
                return null;
            }
        }

        /// <summary>
        /// Requests a list of Subjects for each year
        /// </summary>
        /// <param name="dept">Department no.</param>
        /// <param name="sem">Semester no.</param>
        /// <returns></returns>
        public List<DisciplinaAno> GetDisciplinasAno(int dept, int sem)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getDisciplinasAnoRequestBody DisciplinasAnoBody = new getDisciplinasAnoRequestBody(dept, sem, 0);
                getDisciplinasAnoRequest DisciplinasAnoReq = new getDisciplinasAnoRequest(DisciplinasAnoBody);

                // get response
                getDisciplinasAnoResponse resp = _DSD.getDisciplinasAno(DisciplinasAnoReq);

                // return lists
                return resp.Body.getDisciplinasAnoResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Requests a list of subject classes.
        /// </summary>
        /// <param name="dept">Department no.</param>
        /// <param name="sem">Semester no.</param>
        /// <returns></returns>
        public List<Aula> GetAulas(int dept, int sem)
        {
            try
            {
                // create the request
                getAulasRequestBody AulasBody = new getAulasRequestBody(dept, sem, 0);
                getAulasRequest AulasReq = new getAulasRequest(AulasBody);

                // get response
                getAulasResponse resp = _DSD.getAulas(AulasReq);

                // return lists
                return resp.Body.getAulasResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Requests a list of classes.
        /// </summary>
        /// <param name="dept">Department no.</param>
        /// <param name="sem">Semester no.</param>
        /// <returns></returns>
        public List<Turma> GetTurmas(int dept, int sem)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getTurmasRequestBody TurmasBody = new getTurmasRequestBody(dept, sem, 0);
                getTurmasRequest TurmasReq = new getTurmasRequest(TurmasBody);

                // get response
                getTurmasResponse resp = _DSD.getTurmas(TurmasReq);

                // return lists
                return resp.Body.getTurmasResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Resquests a list of the times for each class.
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="sem"></param>
        /// <returns></returns>
        public List<TurmaHora> GetTurmaHora(int dept, int sem)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getTurmasHoraRequestBody TurmasHoraBody = new getTurmasHoraRequestBody(dept, sem, 0);
                getTurmasHoraRequest TurmasHoraReq = new getTurmasHoraRequest(TurmasHoraBody);

                // get response
                getTurmasHoraResponse resp = _DSD.getTurmasHora(TurmasHoraReq);

                // return lists
                return resp.Body.getTurmasHoraResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Resquests a list of classrooms.
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public List<Sala> GetSalas(int dept)
        {
            // if class has been disposed of, exit.
            if (_disposed) return null;

            try
            {
                // create the request
                getSalasRequestBody SalasBody = new getSalasRequestBody(dept, 0);
                getSalasRequest SalasReq = new getSalasRequest(SalasBody);

                // get response
                getSalasResponse resp = _DSD.getSalas(SalasReq);

                // return lists
                return resp.Body.getSalasResult.ToList();
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Callback for server certificate validation
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="cert">X509Certificate</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="error">SslPolicyErrors error</param>
        /// <returns>true</returns>
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        #region Disposing

        // Release all memory used by this class
        public void Dispose()
        {
            Dispose(true);

            // Tells the GC not to finalize this class, since that will be
            // dealt with the Dispose(true) call
            GC.SuppressFinalize(this);
        }

        // Dispose of other resources (if needed)
        public void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                // free memory
                if (disposing) { }
            }

            // Set the class as disposed. This is a temporary state as the
            // class will be destroyed eventually
            _disposed = true;
        }
        #endregion
    }
}
