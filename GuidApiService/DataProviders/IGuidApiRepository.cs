namespace GuidApiService.DataProviders
{
    /// <summary>
    /// Data access interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGuidApiRepository<T> where T : class
    {
        /// <summary>
        /// Get entity from datastore by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<T> Get(string guid);

        /// <summary>
        /// Create entity in the datastore
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> Create(T entity);

        /// <summary>
        /// Update entity in the datastore
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(T entity);

        /// <summary>
        /// Delete entity in the datastore by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<bool> Delete(string guid);
    }
}
